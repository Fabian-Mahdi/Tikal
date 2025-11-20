import { TokenDto } from "../../dtos/token-dto";

export const testTokenDtos: TokenDto[] = [
  { accessToken: "access token" },
  { accessToken: "" },
  { accessToken: "2$)38$)sD" },
  {
    accessToken:
      "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris eu suscipit metus. Nam imperdiet nunc et dui lacinia, tincidunt malesuada.",
  },
];
