import { useReact_useContext_37FA55CF, useFeliz_React__React_useState_Static_1505, React_contextProvider_34D9BBBD, React_createContext_Z10F951C2 } from "../fable_modules/Feliz.2.7.0/React.fs.js";
import { createElement } from "react";
import React from "react";
import * as react from "react";
import { ofArray, singleton } from "../fable_modules/fable-library.4.5.0/List.js";
import { singleton as singleton_1, delay, toList } from "../fable_modules/fable-library.4.5.0/Seq.js";
import { Interop_reactApi } from "../fable_modules/Feliz.2.7.0/./Interop.fs.js";
import { FSharpResult$2 } from "../fable_modules/fable-library.4.5.0/Choice.js";
import { isException } from "../fable_modules/fable-library.4.5.0/Types.js";

export const requestContext = React_createContext_Z10F951C2("Request");

export function AppLayout(props) {
    let xs_1, xs, children;
    return React_contextProvider_34D9BBBD(requestContext, props.req, (xs_1 = [(xs = [props.req.Link({
        children: singleton(createElement("h1", {
            children: ["Fable Universal Express Demo"],
        })),
        href: "/",
    }), (children = toList(delay(() => {
        ["className", "content"];
        return singleton_1(props.content);
    })), createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    }))], react.createElement(react.Fragment, {}, ...xs))], react.createElement(react.Fragment, {}, ...xs_1)));
}

export function Counter() {
    const patternInput = useFeliz_React__React_useState_Static_1505(0);
    const setCount = patternInput[1];
    const count = patternInput[0] | 0;
    const req = useReact_useContext_37FA55CF(requestContext);
    const xs_1 = [createElement("h2", {
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
    })];
    return react.createElement(react.Fragment, {}, ...xs_1);
}

export function Button(props) {
    return createElement("input", {
        type: "submit",
        value: props.title,
    });
}

export function Test() {
    const req = useReact_useContext_37FA55CF(requestContext);
    const xs_2 = [createElement("h2", {
        children: "Form POST Demo",
    }), req.Form({
        action: "/test_post",
        children: ofArray([createElement("input", {
            type: "text",
            name: "test",
        }), createElement(Button, {
            title: "Submit",
        })]),
        method: "POST",
    })];
    return react.createElement(react.Fragment, {}, ...xs_2);
}

export function verifyPost(value) {
    let s;
    if (value == null) {
        return new FSharpResult$2(1, ["No value provided."]);
    }
    else if ((s = value, s.length >= 5)) {
        const s_1 = value;
        return new FSharpResult$2(0, [s_1]);
    }
    else {
        return new FSharpResult$2(1, ["The value must be at least 6 characters long."]);
    }
}

export function errorHandler(err, req, res, next) {
    if (isException(err)) {
        const ex = err;
        console.log(ex.message);
        const message = ex.message;
        res.status(500);
        const value_1 = res.send(message);
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
        let component$0027;
        const matchValue = verifyPost(test);
        if (matchValue.tag === 1) {
            const msg = matchValue.fields[0];
            res_3.status(400);
            component$0027 = createElement("p", {
                children: [msg],
            });
        }
        else {
            const value_2 = matchValue.fields[0];
            component$0027 = createElement("p", {
                children: ["Value: " + value_2],
            });
        }
        const value_6 = res_3.renderComponent(component$0027);
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

