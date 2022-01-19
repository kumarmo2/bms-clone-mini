import { BACKEND_BASE_URL } from "../../../constants";

export const fetchShowsForCity = (cityId) => {
  return fetch(`${BACKEND_BASE_URL}/api/shows/cities/${cityId}`);
};
