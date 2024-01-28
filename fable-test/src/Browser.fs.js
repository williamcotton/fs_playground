import browser_express from "browser-express";
import express_link from "./browser/middleware/express-link.js";
import react_renderer from "./browser/middleware/react-renderer.js";
import { createElement } from "react";
import { universalApp, AppLayout } from "./App.fs.js";
import { printf, toConsole } from "../fable_modules/fable-library.4.5.0/String.js";

export const app = browser_express();

app.use(express_link());

app.use(react_renderer({
    app: app,
    appLayout: (params) => createElement(AppLayout, {
        params: params,
    }),
}));

universalApp(app);

app.listen(3000, () => {
    toConsole(printf("Listening on port 3000"));
});

