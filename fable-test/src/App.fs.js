import { useReact_useContext_37FA55CF, useFeliz_React__React_useState_Static_1505, React_contextProvider_34D9BBBD, React_createContext_Z10F951C2 } from "../fable_modules/Feliz.2.7.0/React.fs.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { Interop_reactApi } from "../fable_modules/Feliz.2.7.0/./Interop.fs.js";
import { ofArray } from "../fable_modules/fable-library.4.5.0/List.js";

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
        if (test == null) {
            const value_6 = res_2.renderComponent(createElement("p", {
                children: ["No value"],
            }));
        }
        else {
            const value_2 = test;
            const value_4 = res_2.renderComponent(createElement("p", {
                children: ["Value: " + value_2],
            }));
        }
    });
}

