import React from "react";
import Shows from "./pages/Shows";
import CITIES from "./constants/cities";

function App() {
  return (
    <div>
      <header>BMS</header>
      <Shows cityId={CITIES.Mumbai} />
    </div>
  );
}

export default App;
