import json
from flask import Flask, request, jsonify
from flask_sqlalchemy import SQLAlchemy
import os

app = Flask(__name__)
file_path = os.path.abspath(os.getcwd())+"\database.db"
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///' + file_path
db = SQLAlchemy(app)


class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    email = db.Column(db.String(50), nullable=False)
    login = db.Column(db.String(100), nullable=False)
    password = db.Column(db.String(100), nullable=False)

class Letter(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(50), nullable=False)
    body = db.Column(db.String(1000), nullable=False)
    sender = db.Column(db.Integer, nullable=False)
    recipient = db.Column(db.Integer, nullable=False)
    status = db.Column(db.String(30), nullable=False)

@app.route('/get_user', methods=['GET'])
def get_user():
  try:
    args = request.args
    login = args.get("login")
    password = args.get("password")
    user_object = User.query.filter_by(login = login, password = password).first()
    if user_object == None:
      return jsonify({'message': 'Nothing found'}), 404 
    else:
      answer = {"id": user_object.id, "email": user_object.email, "login": user_object.login, "password": user_object.password}
      return answer
  except:
     return jsonify({'message': 'Bad request'}), 404 

@app.route('/get_received_letters', methods=['GET'])
def get_received_letters():
  try:
    args = request.args
    recipient_email = args.get("email")
    recipient = User.query.filter_by(email = recipient_email).first().id
    letters = Letter.query.filter_by(recipient = recipient, status = 'sent')
    if letters == None:
      return jsonify({'message': 'Nothing found'}), 404 
    else:
      data = []
      for letter in letters:
        json_letter = {"id": letter.id, "title": letter.title, "body": letter.body, "sender": (User.query.filter_by(id = letter.sender).first()).email, "recipient": recipient_email, "status": letter.status}
        data.append(json_letter)
      return data
  except:
    return jsonify({'message': 'Bad request'}), 404 
    
@app.route('/get_sent_letters', methods=['GET'])
def get_sent_letters():
  try:
    args = request.args
    sender_email = args.get("email")
    sender = User.query.filter_by(email = sender_email).first().id
    letters = Letter.query.filter_by(sender = sender, status = 'sent')
    if letters == None:
      return jsonify({'message': 'Nothing found'}), 404 
    else:
      data = []
      for letter in letters:
        json_letter = {"id": letter.id, "title": letter.title, "body": letter.body, "recipient": (User.query.filter_by(id = letter.recipient).first()).email, "sender": sender_email, "status": letter.status}
        data.append(json_letter)
      return data
  except:
    return jsonify({'message': 'Bad request'}), 404 

@app.route('/send_letter', methods=['POST'])
def send_letter():
  try:
    data = json.loads(request.data)
    sender_email = data['sender']
    recipient_email = data['recipient']
    sender_object = User.query.filter_by(email = sender_email).first()
    recipient_object = User.query.filter_by(email = recipient_email).first()
    new_letter = Letter(title = data['title'], body = data['body'], sender = sender_object.id,
                        recipient = recipient_object.id, status = data['status'])
    db.session.add(new_letter)
    db.session.commit()
    return jsonify({'message': 'Letter send'}), 201
  except:
    return jsonify({'message': 'Bad request'}), 404 
    
@app.route('/create_user', methods=['POST'])
def create_user():
    data = json.loads(request.data)
    email = data['email']
    login = data['login']
    password = data['password']
    if User.query.filter_by(email = email).first() != None:
       return jsonify({'message': 'Email is busy'}), 404
    new_user = User(email = email, password = password, login = login)
    db.session.add(new_user)
    db.session.commit()
    return jsonify({'message': 'Verse added'}), 201


if __name__ == '__main__':
    app.run(debug=True)