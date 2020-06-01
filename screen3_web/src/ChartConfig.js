const ChartConfig = {
  general: {
    stockWindow: 40000,
    zone: "current",
  },
  indicatorSettings: {
    ema10: {
      parameter: "ema,10",
      ownPane: false,
      color: "#555",
    },
    ema20: {
      parameter: "ema,20",
      ownPane: false,
      color: "#002d99",
    },
    ema50: {
      parameter: "ema,50",
      ownPane: false,
      color: "#9d39f3",
    },
    sma100: {
      parameter: "sma,100",
      color: "#ff00ff",
      ownPane: false,
    },
    sma200: {
      parameter: "sma,200",
      ownPane: false,
      color: "#f02b2b",
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
      color: "#000000",
    },
    closemain: {
      parameter: "closemain",
      color: "#FFFF55",
      ownPane: false,
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
      yAxisName: "stochastic_yaxis",
      parameter: "stochastic,14,3",
      ownPane: true,
      color: "#FFAA00",
      colorK: "#d500d5",
      colorD: "#00aa2b",
      height: 120,
      threshold1: 30,
      threshold2: 70,
    },
    rsi: {
      yAxisName: "rsi_yaxis",
      parameter: "rsi,6",
      ownPane: true,
      color: "#7F2AFF",
      colorRsi: "#AAFFFF",
      height: 100,
    },
    william: {
      yAxisName: "willaim_yaxis",
      parameter: "william,14",
      ownPane: true,
      color: "#5500c5",
      height: 100,
      threshold1: -20,
      threshold2: -80,
    },
  },
};

export default ChartConfig;
