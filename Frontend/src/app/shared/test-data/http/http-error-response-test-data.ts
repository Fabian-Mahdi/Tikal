export const testHttpErrorResponses: { status: number; statusText: string }[] = [
  {
    status: 400,
    statusText: "Bad Request",
  },
  {
    status: 401,
    statusText: "Unauthorized",
  },
  {
    status: 402,
    statusText: "Payment Required",
  },
  {
    status: 403,
    statusText: "Forbidded",
  },
  {
    status: 404,
    statusText: "Not Found",
  },
  {
    status: 405,
    statusText: "Method Not Allowed",
  },
  {
    status: 406,
    statusText: "Not Accetpable",
  },
  {
    status: 409,
    statusText: "Conflict",
  },
  {
    status: 500,
    statusText: "Internal Server Error",
  },
];
