import RequestHelper from "../util/RequestHelper";

export class ScreenService {
  static stockList = [];

  SubmitScreen_MACD_William(requestBody) {
    console.log("request: ", requestBody);
    const req = new RequestHelper().getIntance();

    return req.post("screen/macd_william/sun", requestBody);
  }
}

export default ScreenService;
