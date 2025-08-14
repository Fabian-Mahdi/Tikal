import { Result } from "neverthrow";
import * as Sentry from "@sentry/angular";

export abstract class UseCase<TArgs extends unknown[], TResult, TError> {
  protected abstract name: string;

  async call(...args: TArgs): Promise<Result<TResult, TError>> {
    return await Sentry.startSpan({ name: this.name, op: "task" }, async () => {
      return await this.inner(...args);
    });
  }

  abstract inner(...args: TArgs): Promise<Result<TResult, TError>>;
}
