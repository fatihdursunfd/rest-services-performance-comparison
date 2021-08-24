package main

import (
	"context"
	"encoding/json"
	"fmt"
	"github.com/gorilla/mux"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
	"time"
)

type Address struct {
	State      string
	PostalCode string
}

type User struct {
	UserId   int
	Name     string
	SurName  string
	Age      int
	Birthday time.Time
	Address  Address
}

var collection *mongo.Collection
var ctx context.Context

func handleRequests() {
	myRouter := mux.NewRouter().StrictSlash(true)
	myRouter.HandleFunc("/user/create", createUser).Methods("POST")
	myRouter.HandleFunc("/user/delete/{id}", deleteUser).Methods("DELETE")
	myRouter.HandleFunc("/user/get/{id}", getUserById)

	log.Fatal(http.ListenAndServe(":10000", myRouter))
}

// GET BY ID
func getUserById(w http.ResponseWriter, r *http.Request) {

	vars := mux.Vars(r)
	value := vars["id"]

	id, _ := strconv.Atoi(value)

	cur := collection.FindOne(context.Background(), bson.D{primitive.E{Key: "UserId", Value: id}})
	fmt.Println(cur)

	result := User{}

	err := cur.Decode(&result)
	fmt.Println(err)

	if err != nil {
		log.Fatal(err)
	}
	fmt.Println(result)
	json.NewEncoder(w).Encode(result)

}

// POST
func createUser(w http.ResponseWriter, r *http.Request) {

	reqBody, _ := ioutil.ReadAll(r.Body)

	var user User
	json.Unmarshal(reqBody,&user)
	json.NewEncoder(w).Encode(user)

	res, err := collection.InsertOne(ctx, user)

	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("Inserted document: ", res.InsertedID)
}

// DELETE
func deleteUser(w http.ResponseWriter, r *http.Request) {

	vars := mux.Vars(r)
	value := vars["id"]
	id, _ := strconv.Atoi(value)

	deleteResult, err := collection.DeleteMany(ctx, bson.D{primitive.E{Key: "UserId", Value: id}})

	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("Deleted %v documents in the trainers collection\n", deleteResult.DeletedCount)
}

func Connect() *mongo.Client {

	uri := "mongodb+srv://fatihdursunfd:Fatih.6116@cluster0.xu9mo.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"
	ctx, cancel := context.WithTimeout(context.Background(), 20*time.Second)
	defer cancel()

	client, err := mongo.Connect(ctx, options.Client().ApplyURI(uri))
	if err != nil {
		panic(err)
	}
	fmt.Println("Successfully connected and pinged.")
	return client
}

func main() {

	client := Connect()
	collection = client.Database("fd").Collection("users")

	handleRequests()

}
