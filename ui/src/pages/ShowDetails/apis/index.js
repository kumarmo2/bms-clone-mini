import { BACKEND_BASE_URL } from "../../../constants";

export const fetchShowDetails = (showId) => {
  return fetch(`${BACKEND_BASE_URL}/api/shows/${showId}`);
};
