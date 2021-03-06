import RequestHelper from "../util/RequestHelper";

export class ScreenService {
  static stockList = [];

  SubmitScreen(code, screenType, options, start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.post(
      `screen/${screenType}/${code}?start=${start}&end=${end}`,
      options
    );
  }
}
export default ScreenService;
