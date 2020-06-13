import RequestHelper from "../util/RequestHelper";

export class TradeService {
  createNewAccount(id, name) {
    const req = new RequestHelper().getIntance();
    return req.post(`trade/account`, {
      id,
      name,
    });
  }

  getAllAccount() {
    const req = new RequestHelper().getIntance();
    return req.get(`trade/account`);
  }

  deleteAccount(id) {
    const req = new RequestHelper().getIntance();
    return req.delete(`trade/account/${id}`);
  }

  openPositionAccount(accountId, request) {
    const req = new RequestHelper().getIntance();
    return req.post(`trade/trade/${accountId}`, request);
  }

  getAccountDetails(accountId) {
    const req = new RequestHelper().getIntance();
    return req.get(`trade/trade/${accountId}`);
  }
}

export default TradeService;
