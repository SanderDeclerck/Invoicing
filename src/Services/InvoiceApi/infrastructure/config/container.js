import { createContainer } from "awilix";
import resolveEndpoints from "./endpoints.js";

var container = createContainer();

resolveEndpoints(container);

export default container;
