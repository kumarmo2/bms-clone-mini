import React from "react";
import Shows from "./pages/Shows";
import ShowDetails from "./pages/ShowDetails/components/ShowDetails";
import CITIES from "./constants/cities";
import { Routes, Route } from "react-router-dom";

function App() {
  return (
    <div>
      <header>BMS</header>
      <Routes>
        <Route path="/shows" element={<Shows cityId={CITIES.Mumbai} />}>
          <Route path=":showId" element={<ShowDetails />} />
        </Route>
      </Routes>
    </div>
  );
}

export default App;
