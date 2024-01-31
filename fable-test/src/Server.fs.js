import express from "express";
import cookie_session from "cookie-session";
import csurf from "csurf";
import express_link from "./server/middleware/express-link.js";
import react_renderer from "./server/middleware/react-renderer.js";
import { createElement } from "react";
import { universalApp, AppLayout } from "./App.fs.js";
import { printf, toConsole } from "../fable_modules/fable-library.4.5.0/String.js";

export const sessionSecret = "tempsecret";

export const app = express();

app.use(express.static("public"));

app.use(cookie_session({
    name: "session",
    sameSite: "lax",
    secret: sessionSecret,
}));

app.use(csurf());

app.use(express_link({
    defaultTitle: "Fable Universal Express Demo",
}));

app.use(react_renderer({
    appLayout: (params) => createElement(AppLayout, {
        params: params,
    }),
}));

universalApp(app);

app.listen(3000, () => {
    toConsole(printf("Listening on port 3000"));
});

