import { LogType } from "@_types/debug";

export const logger = (
  logType: LogType = LogType.INFO,
  message: string = "?",
  object?: unknown,
  table: boolean = false
) => {
  const inProd = import.meta.env.PROD;
  if (inProd) return;

  if (table) {
    console.log(logType + message);
    console.table(object);
  } else {
    console.log(logType + message + " ->", object);
  }
};
