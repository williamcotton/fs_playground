import { createRoot } from "react-dom/client";
import { createElement } from "react";
import { Counter } from "./App.fs.js";

export const root = createRoot(document.getElementById("root"));

root.render(createElement(Counter, null));

