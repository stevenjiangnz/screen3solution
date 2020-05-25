import axios from "axios";

// axios.defaults.headers.common["Authorization"] = AUTH_TOKEN;

export class RequestHelper {
  constructor() {
    console.log(
      "request helper constrcutor called: ",
      process.env.REACT_APP_API_BASE_URL
    );

    axios.defaults.baseURL = process.env.REACT_APP_API_BASE_URL;
    //     axios.defaults.headers.post["Content-Type"] =
    //   "application/x-www-form-urlencoded";
  }

  getIntance() {
    return axios.create();
  }
}

export default RequestHelper;
