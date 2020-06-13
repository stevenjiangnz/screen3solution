export class TickerHelper {
  static ConvertTickers(tickers) {
    const convertedTickers = [];

    tickers.forEach((t) => {
      convertedTickers.push([t.p_Stamp, t.o, t.h, t.l, t.c]);
    });

    return convertedTickers;
  }

  static ConvertSingleValueIndicator(Indicator) {
    const convertedIndicators = [];

    Indicator.forEach((ind) => {
      convertedIndicators.push([ind.p_Stamp, ind.v]);
    });

    return convertedIndicators;
  }

  static ConvertBBIndicator(Indicator) {
    const convertedIndicators = {
      high: [],
      mid: [],
      low: [],
    };

    Indicator.forEach((ind) => {
      convertedIndicators.high.push([ind.p_Stamp, ind.high]);
      convertedIndicators.mid.push([ind.p_Stamp, ind.mid]);
      convertedIndicators.low.push([ind.p_Stamp, ind.low]);
    });

    return convertedIndicators;
  }

  static ConvertStochasticIndicator(Indicator) {
    const convertedIndicators = {
      k: [],
      d: [],
    };

    Indicator.forEach((ind) => {
      convertedIndicators.k.push([ind.p_Stamp, ind.k]);
      convertedIndicators.d.push([ind.p_Stamp, ind.d]);
    });

    return convertedIndicators;
  }

  static ConvertMACDIndicator(Indicator) {
    const convertedIndicators = {
      macd: [],
      signal: [],
      hist: [],
    };

    Indicator.forEach((ind) => {
      convertedIndicators.macd.push([ind.p_Stamp, ind.macd]);
      convertedIndicators.signal.push([ind.p_Stamp, ind.signal]);
      convertedIndicators.hist.push([ind.p_Stamp, ind.hist]);
    });

    return convertedIndicators;
  }

  static ConvertADXIndicator(Indicator) {
    const convertedIndicators = {
      adx: [],
      di_plus: [],
      di_minus: [],
    };

    Indicator.forEach((ind) => {
      convertedIndicators.adx.push([ind.p_Stamp, ind.adx]);
      convertedIndicators.di_plus.push([ind.p_Stamp, ind.di_plus]);
      convertedIndicators.di_minus.push([ind.p_Stamp, ind.di_minus]);
    });

    return convertedIndicators;
  }

  static ConvertHeikinIndicator(Indicator) {
    const convertedTickers = [];

    Indicator.forEach((t) => {
      convertedTickers.push([t.p_Stamp, t.open, t.high, t.low, t.close]);
    });

    return convertedTickers;
  }

  static formatNum(num, dp) {
    return (Math.round(num * 100) / 100).toFixed(dp);
  }
}

export default TickerHelper;
