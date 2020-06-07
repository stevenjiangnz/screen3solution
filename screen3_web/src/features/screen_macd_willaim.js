export default {
  name: "Screen_MACD_William",
  start: 20020101,
  end: 20200201,
  options: {
    WILLIAM_BUY_LEVEL: -80,
    WILLIAM_SELL_LEVEL: -20,
    MACD_BUY_LEVEL: 0,
    MACD_SELL_LEVEL: 0,
    DECLUSTER: 2,
    DIRECTION: "BUY,SELL",
  },
  stocks: ["xao", "sun", "rio", "amp"],
};
