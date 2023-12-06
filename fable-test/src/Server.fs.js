import express from "express";
import { renderToString } from "react-dom/server";
import { createElement } from "react";
import { Counter } from "./App.fs.js";
import { printf, toConsole } from "../fable_modules/fable-library.4.5.0/String.js";

export const app = express();

export const d = renderToString(createElement(Counter, null));

toConsole(printf("%s"))(d);

app.get("/", (req, res, _arg) => {
    const value = res.send(d);
});

app.listen(3000, () => {
    toConsole(printf("Listening on port 3000"));
});

