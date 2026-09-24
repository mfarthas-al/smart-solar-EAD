import axios from "axios";

// Base URL of the Web Service API
const api = axios.create({
  baseURL: "http://localhost:5299/api",
});

export default api;
