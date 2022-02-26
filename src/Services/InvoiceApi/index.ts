import build from "./app";

const app = build();

app.listen(process.env.PORT || 3000);
