import RequestHelper from "../util/RequestHelper";

export class ScreenService {
  static stockList = [];

  SubmitScreen(requestBody) {
    console.log("request: ", requestBody);
    const req = new RequestHelper().getIntance();

    return req.post("screen", requestBody);
  }
}

export default ScreenService;
