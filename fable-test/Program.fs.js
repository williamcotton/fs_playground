import express from "express";
import { createElement } from "react";
import React from "react";
import { useFeliz_React__React_useState_Static_1505 } from "./fable_modules/Feliz.2.7.0/React.fs.js";
import { ofArray } from "./fable_modules/fable-library.4.5.0/List.js";
import { Interop_reactApi } from "./fable_modules/Feliz.2.7.0/./Interop.fs.js";
import { renderToString } from "react-dom/server";
import { printf, toConsole } from "./fable_modules/fable-library.4.5.0/String.js";

export const app = express();

export function Counter() {
    const patternInput = useFeliz_React__React_useState_Static_1505(0);
    const setCount = patternInput[1];
    const count = patternInput[0] | 0;
    const children = ofArray([createElement("h1", {
        children: [count],
    }), createElement("button", {
        children: "Increment",
        onClick: (_arg) => {
            setCount(count + 1);
        },
    })]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

export const d = renderToString(createElement(Counter, null));

toConsole(printf("%s"))(d);

app.get("/", (req, res, _arg) => {
    const value = res.send(d);
});

app.listen(3000, () => {
    toConsole(printf("Listening on port 3000"));
});

