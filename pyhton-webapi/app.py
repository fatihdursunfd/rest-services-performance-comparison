from flask import Flask , jsonify
from flask_restx import Api, Resource, fields , marshal
from pymongo import MongoClient
import json
from datetime import datetime

app = Flask(__name__)
api = Api(app, version='1.0', title='Python WEB API')
ns = api.namespace('users', description='endpoints')

connection_string = ""

try:
    cluster = MongoClient(connection_string)
    db = cluster['fd']
    collection = db['users']
    print("connection succed")

except Exception as ex:
    print(ex)

address = api.model('Address' , {
        'State' : fields.String(required=True, description='The task details'),
        'PostalCode' : fields.String(required=True, description='The task details')
})

user = api.model('User', {
    'UserId': fields.Integer(required=True, description='The task unique identifier'),
    'Name': fields.String(required=True, description='The task details'),
    'SurName': fields.String(required=True, description='The task details'),
    'Birthday' : fields.DateTime(required=True, description='The task details'),
    'Age':fields.Integer(required=True, description='The task details'),
    'address' : fields.Nested(address , skip_none=True)
})
def to_json(_user):
    _user = json.dumps(marshal(_user, user))
    _user = _user.replace("[", "").replace("]", "")
    _user = json.loads(_user)
    return _user
def string_to_date(time):
    time = time.replace("T", "-")
    time = time.replace("Z", "")
    time = datetime.strptime(time, "%Y-%m-%d-%H:%M:%S.%f")
    return time

@ns.route('/')
class Post(Resource):
    @ns.expect(user)
    @ns.marshal_with(user, code=201)
    def post(self):
        collection.insert_one(api.payload)
        return "Ok", 201


@ns.route('/<int:id>')
class Endpoints(Resource):
    @ns.marshal_list_with(user)
    def get(self,id):
        _user = to_json(list(collection.find({"UserId": id})))
        return _user

    @ns.response(204, 'Todo deleted')
    def delete(self, id):
        query = {
            "UserId": id
        }
        collection.delete_one(query)
        return '', 204

    @ns.expect(user)
    @ns.marshal_with(user)
    def put(self, id):
        query = {"UserId": id}
        _user = to_json(list(collection.find(query)))

        if api.payload['Name'] != "string" or "":
            _user['Name'] = api.payload['Name']
        if api.payload['SurName'] != "string" or "":
            _user['SurName'] = api.payload['SurName']
        time = string_to_date(api.payload['Birthday'])
        if time.date() < datetime.today().date():
            _user['Birthday'] = api.payload['Birthday']
        if api.payload['address']['State'] != "string" or "":
            _user['address']['State'] = api.payload['address']['State']
        if api.payload['address']['PostalCode'] != "string" or "":
            _user['address']['PostalCode'] = api.payload['address']['PostalCode']

        update = {"$set": _user}
        collection.update_one(query,update)


if __name__ == "__main__":
    app.run()

