import React from "react";

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <button
          className="btn btn-primary"
          onClick={() => {
            alert("test buttone clicked");
          }}
        >
          test button
        </button>
      </header>
    </div>
  );
}

export default App;
