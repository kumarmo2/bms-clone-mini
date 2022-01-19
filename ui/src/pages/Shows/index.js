import React, { useEffect, useState, useCallback } from "react";
import { fetchShowsForCity } from "./apis";
import { Outlet } from "react-router-dom";

const Shows = ({ cityId }) => {
  const [shows, setShows] = useState([]);

  const fetchAndSetShows = useCallback(async () => {
    let response = await fetchShowsForCity(cityId);
    if (!response) {
      return;
    }
    if (!response.ok) {
      console.log("some error, response: ", response);
      return;
    }
    response = await response.json();

    if (!response.ok || !response.ok.shows || !response.ok.shows.length) {
      console.log("some error, response: ", response);
      return;
    }
    setShows(response.ok.shows);
  }, [cityId, setShows]);

  useEffect(() => {
    const fn = async () => {
      if (!cityId) {
        return;
      }
      // TODO: make use of async await.
      try {
        await fetchAndSetShows();
      } catch (err) {
        console.log("error: ", err);
      }
    };
    fn();
  }, [cityId]);

  if (!cityId) {
    return <div>Select City</div>;
  }

  return (
    <div>
      Shows page
      {shows.map((show) => {
        return (
          <div key={show.showId}>
            <p>
              movie: {show.movieName}, startTime: {show.startTime}
            </p>
          </div>
        );
      })}
      <Outlet />
    </div>
  );
};

export default Shows;
