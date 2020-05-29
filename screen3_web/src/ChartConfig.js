const ChartConfig = {
  general: {
    stockWindow: 40000,
    zone: "current",
  },
  indicatorSettings: {
    ema10: {
      parameter: "ema,10",
      ownPane: false,
      color: "#aaa",
    },
    ema20: {
      parameter: "ema,20",
      ownPane: false,
      color: "#FFD455",
    },
    ema50: {
      parameter: "ema,50",
      ownPane: false,
      color: "#61B60C",
    },
    sma100: {
      parameter: "sma,100",
      color: "#00FF00",
      ownPane: false,
    },
    sma200: {
      parameter: "sma,200",
      ownPane: false,
      color: "#0FE4E4",
    },
    // ema10: {
    //   parameter: "ema,10",
    //   ownPane: false,
    //   color: "#AE2EAE",
    // },
    // ema20: {
    //   parameter: "ema,20",
    //   ownPane: false,
    //   color: "#FF0055",
    // },
    bb: {
      parameter: "bb,20,2.5",
      ownPane: false,
      color: "#ffffff",
    },
    closemain: {
      parameter: "closemain",
      color: "#FFFF55",
      ownPane: false,
    },
    rsi: {
      parameter: "rsi,6",
      ownPane: true,
      color: "#7F2AFF",
      colorRsi: "#AAFFFF",
      height: 100,
    },
    adx: {
      parameter: "adx",
      ownPane: true,
      color: "#FF55AA",
      colorAdx: "#FFFFFF",
      colorDiPlus: "#2AFF2A",
      colorDiMinus: "#FF55D4",
      height: 180,
    },
    macd: {
      parameter: "macd,26,12,9",
      ownPane: true,
      color: "#FF55AA",
      colorMacd: "#2AFF2A",
      colorSignal: "#FF55D4",
      colorHist: "#E1E1E1",
      height: 150,
    },
    heikin: {
      parameter: "heikin",
      ownPane: true,
      color: "#AAAAFF",
      height: 150,
    },
    stochastic: {
      parameter: "stochastic,14,3",
      ownPane: true,
      color: "#FFAA00",
      colorK: "#2AFF2A",
      colorD: "#FF55D4",
      height: 120,
      threshold1: 30,
      threshold2: 70,
    },
    william: {
      parameter: "william,14",
      ownPane: true,
      color: "#AAFF2A",
      colorWilliam: "#FFFF2A",
      height: 100,
      threshold1: -20,
      threshold2: -80,
    },
  },
};

export default ChartConfig;
