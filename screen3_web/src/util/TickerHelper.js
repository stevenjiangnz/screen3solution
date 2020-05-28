export class TickerHelper {
  static ConvertTickers(tickers) {
    const convertedTickers = [];

    tickers.forEach((t) => {
      convertedTickers.push([t.p_Stamp, t.o, t.h, t.l, t.c]);
    });

    return convertedTickers;
  }
}

export default TickerHelper;
