import axios from "axios";

// axios.defaults.headers.common["Authorization"] = AUTH_TOKEN;

export class RequestHelper {
  constructor() {
    console.log(
      "request helper constrcutor called: ",
      process.env.REACT_APP_API_BASE_URL,
      process.env.REACT_APP_API_ACCESS_KEY
    );

    axios.defaults.baseURL = process.env.REACT_APP_API_BASE_URL;
    axios.defaults.headers.common["x-api-key"] =
      process.env.REACT_APP_API_ACCESS_KEY;
    axios.defaults.headers.common["Content-Type"] = "application/json";
    axios.defaults.headers.common["Access-Control-Allow-Origin"] = "*";
    // axios.defaults.headers.common["Access-Control-Allow-Methods"] =
    //   "DELETE, POST, PUT, GET, OPTIONS";
    // axios.defaults.headers.common["Access-Control-Allow-Headers"] =
    //   "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With, x-api-key";
    // axios.defaults.headers.common["Access-Control-Allow-Credentials"] = true;
  }

  getIntance() {
    return axios.create();
  }
}

export default RequestHelper;
