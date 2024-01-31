import { useReact_useContext_37FA55CF, useFeliz_React__React_useState_Static_1505, React_contextProvider_34D9BBBD, React_createContext_Z10F951C2 } from "../fable_modules/Feliz.2.7.0/React.fs.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Interop_reactApi } from "../fable_modules/Feliz.2.7.0/./Interop.fs.js";
import { ofArray } from "../fable_modules/fable-library.4.5.0/List.js";
import { isException } from "../fable_modules/fable-library.4.5.0/Types.js";
import { printf, toText } from "../fable_modules/fable-library.4.5.0/String.js";

export const requestContext = React_createContext_Z10F951C2("Request");

export function AppLayout(appLayoutInputProps) {
    let xs, children_2;
    const params = appLayoutInputProps.params;
    const contentElement = params.content;
    const req = params.req;
    return React_contextProvider_34D9BBBD(requestContext, req, (xs = [(children_2 = ofArray([createElement("h1", {
        children: ["Fable Universal Express Demo"],
    }), createElement("div", {
        children: Interop_reactApi.Children.toArray([contentElement]),
    })]), createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children_2)),
    }))], react.createElement(react.Fragment, {}, ...xs)));
}

export function Counter() {
    const patternInput = useFeliz_React__React_useState_Static_1505(0);
    const setCount = patternInput[1];
    const count = patternInput[0] | 0;
    const req = useReact_useContext_37FA55CF(requestContext);
    const children = ofArray([createElement("h2", {
        children: [count],
    }), createElement("button", {
        children: "Increment",
        onClick: (_arg) => {
            setCount(count + 1);
        },
    }), req.Link({
        children: "Test",
        href: "/test",
    })]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

export function Test() {
    const patternInput = useFeliz_React__React_useState_Static_1505(0);
    const setCount = patternInput[1];
    const count = patternInput[0] | 0;
    const req = useReact_useContext_37FA55CF(requestContext);
    const children = ofArray([createElement("h2", {
        children: "Form POST Demo",
    }), req.Form({
        action: "/test_post",
        children: ofArray([createElement("input", {
            type: "text",
            name: "test",
        }), createElement("input", {
            type: "submit",
            value: "Submit",
        })]),
        method: "POST",
    })]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

export function verifyPost(value) {
    if (value == null) {
        return void 0;
    }
    else {
        const value_1 = value;
        if (value_1 === "test") {
            return "test";
        }
        else {
            return void 0;
        }
    }
}

export function errorHandler(err, req, res, next) {
    let arg;
    if (isException(err)) {
        const ex = err;
        console.log(ex.message);
        res.send((arg = ex.message, toText(printf("An error occurred: %s"))(arg)));
    }
    else {
        next();
    }
}

export function notFoundHandler(req, res, next) {
    res.status(404);
    const value_2 = res.renderComponent(createElement("p", {
        children: ["Not found"],
    }));
}

export function universalApp(app) {
    app.get("/", (req, res, _arg) => {
        const value = res.renderComponent(createElement(Counter, null));
    });
    app.get("/test", (req_1, res_1, _arg_1) => {
        const value_1 = res_1.renderComponent(createElement(Test, null));
    });
    app.post("/test_post", (req_2, res_2, _arg_2) => {
        const body = req_2.body;
        const test = req_2.body.test;
        const matchValue = verifyPost(test);
        if (matchValue == null) {
            const value_6 = res_2.renderComponent(createElement("p", {
                children: ["Not valid"],
            }));
        }
        else {
            const value_2 = matchValue;
            const value_4 = res_2.renderComponent(createElement("p", {
                children: ["Value: " + value_2],
            }));
        }
    });
    app.use((req_3, res_3, next) => {
        notFoundHandler(req_3, res_3, next);
    });
    app.use((err, req_4, res_4, next_1) => {
        errorHandler(err, req_4, res_4, next_1);
    });
}

