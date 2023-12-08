import { createElement } from "react";
import React from "react";
import { Interop_reactApi } from "../fable_modules/Feliz.2.7.0/./Interop.fs.js";
import { ofArray } from "../fable_modules/fable-library.4.5.0/List.js";
import { useFeliz_React__React_useState_Static_1505 } from "../fable_modules/Feliz.2.7.0/React.fs.js";

export function AppLayout(appLayoutInputProps) {
    const params = appLayoutInputProps.params;
    const contentElement = params.content;
    const children_2 = ofArray([createElement("h1", {
        children: ["Hello World"],
    }), createElement("div", {
        children: Interop_reactApi.Children.toArray([contentElement]),
    })]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children_2)),
    });
}

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

export function baseRoute(app) {
    app.get("/", (req, res, _arg) => {
        const value = res.renderComponent(createElement(Counter, null));
    });
}

