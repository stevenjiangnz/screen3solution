import axios from "axios";

export class RequestHelper {
  constructor() {
    axios.defaults.baseURL = process.env.REACT_APP_API_BASE_URL;
    axios.defaults.headers.common["x-api-key"] =
      process.env.REACT_APP_API_ACCESS_KEY;
    axios.defaults.headers.common["Content-Type"] = "application/json";
  }

  getIntance() {
    return axios.create();
  }
}

export default RequestHelper;
