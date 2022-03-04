import setupTelemetry from "./infrastructure/telemetry";
import "dotenv/config";

setupTelemetry();

import build from "./app";

build().then(app => app.listen(process.env.PORT || 3000));

