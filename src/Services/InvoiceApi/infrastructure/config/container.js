import { asFunction, createContainer } from "awilix";
import routes from "../../endpoints/http/routes.js";

var container = createContainer();

container.register({
    routes: asFunction(routes)
})

export default container;
