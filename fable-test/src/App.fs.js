import { useReact_useContext_37FA55CF, useFeliz_React__React_useState_Static_1505, React_contextProvider_34D9BBBD, React_createContext_Z10F951C2 } from "../fable_modules/Feliz.2.7.0/React.fs.js";
import { isException, Record } from "../fable_modules/fable-library.4.5.0/Types.js";
import { string_type, record_type, class_type } from "../fable_modules/fable-library.4.5.0/Reflection.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Interop_reactApi } from "../fable_modules/Feliz.2.7.0/./Interop.fs.js";
import { ofArray } from "../fable_modules/fable-library.4.5.0/List.js";

export const requestContext = React_createContext_Z10F951C2("Request");

export class AppLayoutParams extends Record {
    constructor(content, req) {
        super();
        this.content = content;
        this.req = req;
    }
}

export function AppLayoutParams_$reflection() {
    return record_type("App.AppLayoutParams", [], AppLayoutParams, () => [["content", class_type("Fable.React.ReactElement")], ["req", class_type("Express.ExpressReq")]]);
}

export function AppLayout(params) {
    let xs, children_2;
    return React_contextProvider_34D9BBBD(requestContext, params.req, (xs = [(children_2 = ofArray([createElement("h1", {
        children: ["Fable Universal Express Demo"],
    }), createElement("div", {
        children: Interop_reactApi.Children.toArray([params.content]),
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
    }), req.Link({
        children: "Error",
        href: "/error",
    })]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

export class ButtonProps extends Record {
    constructor(title) {
        super();
        this.title = title;
    }
}

export function ButtonProps_$reflection() {
    return record_type("App.ButtonProps", [], ButtonProps, () => [["title", string_type]]);
}

export function Button(props) {
    return createElement("input", {
        type: "submit",
        value: props.title,
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
        }), createElement(Button, new ButtonProps("Submit"))]),
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
    let children;
    if (isException(err)) {
        const ex = err;
        console.log(ex.message);
        const message = ex.message;
        res.status(500);
        const value_3 = res.renderComponent((children = ofArray([createElement("p", {
            children: ["We\'re sorry, something went wrong!"],
        }), createElement("pre", {
            children: [message],
        })]), createElement("div", {
            children: Interop_reactApi.Children.toArray(Array.from(children)),
        })));
    }
    else {
        next();
    }
}

export function universalApp(app) {
    app.get("/", (req, res, _arg) => {
        const value = res.renderComponent(createElement(Counter, null));
    });
    app.get("/test", (req_1, res_1, _arg_1) => {
        const value_1 = res_1.renderComponent(createElement(Test, null));
    });
    app.get("/error", (req_2, res_2, _arg_2) => {
        throw new Error("Some sort of detailed error message");
    });
    app.post("/test_post", (req_3, res_3, _arg_3) => {
        const body = req_3.body;
        const test = req_3.body.test;
        const matchValue = verifyPost(test);
        if (matchValue == null) {
            const value_6 = res_3.renderComponent(createElement("p", {
                children: ["Not valid"],
            }));
        }
        else {
            const value_2 = matchValue;
            const value_4 = res_3.renderComponent(createElement("p", {
                children: ["Value: " + value_2],
            }));
        }
    });
    app.use((req_4, res_4, next) => {
        res_4.status(404);
        const value_9 = res_4.renderComponent(createElement("p", {
            children: ["Page not found"],
        }));
    });
    app.use((err, req_5, res_5, next_1) => {
        errorHandler(err, req_5, res_5, next_1);
    });
}

