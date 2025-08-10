export type LoginError = {
  error_message: string;
};

export class InvalidCredentials implements LoginError {
  error_message = "invalid username or password";
}

export class UnknownError implements LoginError {
  error_message = "an unknown error has occurred";
}
