import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { fetchShowDetails } from "../apis";

const ShowDetails = () => {
  const params = useParams();
  const showId = +params.showId;
  console.log("params: ", params);

  useEffect(() => {
    if (isNaN(showId)) {
      return;
    }
    const fetchShowDetailsFn = async () => {
      let response = await fetchShowDetails(showId);
      if (!response.ok) {
        console.log("response was not ok");
        return;
      }
      response = await response.json();
      if (!response) {
        return;
      }
      if (response.err) {
        console.log("err: ", response.err);
        return;
      }
      console.log("response.ok: ", response.ok);
    };
    fetchShowDetailsFn();
  }, [showId]);

  if (isNaN(showId)) {
    return null;
  }
  return <h1>Show Details Section</h1>;
};

export default ShowDetails;
