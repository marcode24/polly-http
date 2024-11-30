import express from "express";

const PORT = 3000;

const app = express();

const SSP_RESPONSE = {
  folio: "XXXX",
  curp: "XXXX",
  nombre: "OTRO",
  apellido_paterno: "PRUEBA",
  apellido_materno: "PRUEBA",
  fecha_solicitud: "2023-08-08 19:16:52",
  contenido_pdf: "text-base64.pdf",
};

const STATUS_CODES_BAD = [400, 401, 403, 404, 405, 406, 407, 408, 409];
const STATUS_CODES_SERVER = [500, 501, 502, 503, 504, 505, 506, 507, 508];
const STATUS_CODES_GOOD = [200, 201];

app.get("/", (req, res) => {
  res.send("Hello World");
});

app.post("/ssp/:id", (req, res) => {
  // const STATUS_CODES = [...STATUS_CODES_BAD, ...STATUS_CODES_SERVER];
  const STATUS_CODES = [...STATUS_CODES_GOOD];
  const statusCode =
    STATUS_CODES[Math.floor(Math.random() * STATUS_CODES.length)];
  console.log("statusCode", statusCode);
  res.status(statusCode).json(SSP_RESPONSE);
});

app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
