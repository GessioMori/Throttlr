db = db.getSiblingDB("ApiGatewayDB");

db.createUser({
    user: "gateway_user",
    pwd: "gateway_pass",
    roles: [{ role: "readWrite", db: "ApiGatewayDB" }]
});

db.createCollection("routes");
db.routes.insertOne({
    path: "/posts",
    upstreamUrl: "https://jsonplaceholder.typicode.com/posts",
    httpVerb: "GET"
});

db.routes.createIndex(
    { path: 1 },
    { unique: true, background: true }
);