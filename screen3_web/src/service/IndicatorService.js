import RequestHelper from "../util/RequestHelper";

export class IndictorService {
  getTickerList(code, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(`ticker/${code}?type=${type}&start=${start}&end=${end}`);
  }
}

export default IndictorService;
