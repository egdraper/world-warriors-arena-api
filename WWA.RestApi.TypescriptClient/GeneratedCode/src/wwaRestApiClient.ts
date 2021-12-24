/*
 * Code generated by Microsoft (R) AutoRest Code Generator.
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
 */

import * as msRest from "@azure/ms-rest-js";
import * as Models from "./models";
import * as Mappers from "./models/mappers";
import * as Parameters from "./models/parameters";
import { WwaRestApiClientContext } from "./wwaRestApiClientContext";

class WwaRestApiClient extends WwaRestApiClientContext {
  /**
   * Initializes a new instance of the WwaRestApiClient class.
   * @param [options] The parameter options
   */
  constructor(options?: Models.WwaRestApiClientOptions) {
    super(options);
  }

  /**
   * @summary Tests an Access Token
   * @param [options] The optional parameters
   * @returns Promise<msRest.RestResponse>
   */
  testAccessToken(options?: msRest.RequestOptionsBase): Promise<msRest.RestResponse>;
  /**
   * @param callback The callback
   */
  testAccessToken(callback: msRest.ServiceCallback<void>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  testAccessToken(options: msRest.RequestOptionsBase, callback: msRest.ServiceCallback<void>): void;
  testAccessToken(options?: msRest.RequestOptionsBase | msRest.ServiceCallback<void>, callback?: msRest.ServiceCallback<void>): Promise<msRest.RestResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      testAccessTokenOperationSpec,
      callback);
  }

  /**
   * @summary Creates an Access Token
   * @param [options] The optional parameters
   * @returns Promise<Models.CreateAccessTokenResponse>
   */
  createAccessToken(options?: Models.WwaRestApiClientCreateAccessTokenOptionalParams): Promise<Models.CreateAccessTokenResponse>;
  /**
   * @param callback The callback
   */
  createAccessToken(callback: msRest.ServiceCallback<string>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  createAccessToken(options: Models.WwaRestApiClientCreateAccessTokenOptionalParams, callback: msRest.ServiceCallback<string>): void;
  createAccessToken(options?: Models.WwaRestApiClientCreateAccessTokenOptionalParams | msRest.ServiceCallback<string>, callback?: msRest.ServiceCallback<string>): Promise<Models.CreateAccessTokenResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      createAccessTokenOperationSpec,
      callback) as Promise<Models.CreateAccessTokenResponse>;
  }

  /**
   * @summary Gets a list of Current Context
   * @param [options] The optional parameters
   * @returns Promise<Models.GetCurrentContextResponse>
   */
  getCurrentContext(options?: msRest.RequestOptionsBase): Promise<Models.GetCurrentContextResponse>;
  /**
   * @param callback The callback
   */
  getCurrentContext(callback: msRest.ServiceCallback<Models.CurrentContextReadViewModel>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  getCurrentContext(options: msRest.RequestOptionsBase, callback: msRest.ServiceCallback<Models.CurrentContextReadViewModel>): void;
  getCurrentContext(options?: msRest.RequestOptionsBase | msRest.ServiceCallback<Models.CurrentContextReadViewModel>, callback?: msRest.ServiceCallback<Models.CurrentContextReadViewModel>): Promise<Models.GetCurrentContextResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      getCurrentContextOperationSpec,
      callback) as Promise<Models.GetCurrentContextResponse>;
  }

  /**
   * @summary Queries a Game
   * @param [options] The optional parameters
   * @returns Promise<msRest.RestResponse>
   */
  queryGames(options?: Models.WwaRestApiClientQueryGamesOptionalParams): Promise<msRest.RestResponse>;
  /**
   * @param callback The callback
   */
  queryGames(callback: msRest.ServiceCallback<void>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  queryGames(options: Models.WwaRestApiClientQueryGamesOptionalParams, callback: msRest.ServiceCallback<void>): void;
  queryGames(options?: Models.WwaRestApiClientQueryGamesOptionalParams | msRest.ServiceCallback<void>, callback?: msRest.ServiceCallback<void>): Promise<msRest.RestResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      queryGamesOperationSpec,
      callback);
  }

  /**
   * @summary Gets a list of Games
   * @param [options] The optional parameters
   * @returns Promise<Models.GetGamesResponse>
   */
  getGames(options?: Models.WwaRestApiClientGetGamesOptionalParams): Promise<Models.GetGamesResponse>;
  /**
   * @param callback The callback
   */
  getGames(callback: msRest.ServiceCallback<Models.GameSummaryViewModel[]>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  getGames(options: Models.WwaRestApiClientGetGamesOptionalParams, callback: msRest.ServiceCallback<Models.GameSummaryViewModel[]>): void;
  getGames(options?: Models.WwaRestApiClientGetGamesOptionalParams | msRest.ServiceCallback<Models.GameSummaryViewModel[]>, callback?: msRest.ServiceCallback<Models.GameSummaryViewModel[]>): Promise<Models.GetGamesResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      getGamesOperationSpec,
      callback) as Promise<Models.GetGamesResponse>;
  }

  /**
   * @summary Creates a Game
   * @param [options] The optional parameters
   * @returns Promise<Models.CreateGameResponse>
   */
  createGame(options?: Models.WwaRestApiClientCreateGameOptionalParams): Promise<Models.CreateGameResponse>;
  /**
   * @param callback The callback
   */
  createGame(callback: msRest.ServiceCallback<Models.GameReadViewModel>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  createGame(options: Models.WwaRestApiClientCreateGameOptionalParams, callback: msRest.ServiceCallback<Models.GameReadViewModel>): void;
  createGame(options?: Models.WwaRestApiClientCreateGameOptionalParams | msRest.ServiceCallback<Models.GameReadViewModel>, callback?: msRest.ServiceCallback<Models.GameReadViewModel>): Promise<Models.CreateGameResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      createGameOperationSpec,
      callback) as Promise<Models.CreateGameResponse>;
  }

  /**
   * @summary Gets a Game
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<Models.GetGameResponse>
   */
  getGame(id: string, options?: msRest.RequestOptionsBase): Promise<Models.GetGameResponse>;
  /**
   * @param id
   * @param callback The callback
   */
  getGame(id: string, callback: msRest.ServiceCallback<Models.GameReadViewModel>): void;
  /**
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  getGame(id: string, options: msRest.RequestOptionsBase, callback: msRest.ServiceCallback<Models.GameReadViewModel>): void;
  getGame(id: string, options?: msRest.RequestOptionsBase | msRest.ServiceCallback<Models.GameReadViewModel>, callback?: msRest.ServiceCallback<Models.GameReadViewModel>): Promise<Models.GetGameResponse> {
    return this.sendOperationRequest(
      {
        id,
        options
      },
      getGameOperationSpec,
      callback) as Promise<Models.GetGameResponse>;
  }

  /**
   * @summary Updates a Game
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<Models.UpdateGameResponse>
   */
  updateGame(id: string, options?: Models.WwaRestApiClientUpdateGameOptionalParams): Promise<Models.UpdateGameResponse>;
  /**
   * @param id
   * @param callback The callback
   */
  updateGame(id: string, callback: msRest.ServiceCallback<Models.GameReadViewModel>): void;
  /**
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  updateGame(id: string, options: Models.WwaRestApiClientUpdateGameOptionalParams, callback: msRest.ServiceCallback<Models.GameReadViewModel>): void;
  updateGame(id: string, options?: Models.WwaRestApiClientUpdateGameOptionalParams | msRest.ServiceCallback<Models.GameReadViewModel>, callback?: msRest.ServiceCallback<Models.GameReadViewModel>): Promise<Models.UpdateGameResponse> {
    return this.sendOperationRequest(
      {
        id,
        options
      },
      updateGameOperationSpec,
      callback) as Promise<Models.UpdateGameResponse>;
  }

  /**
   * @summary Deletes a Game
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<msRest.RestResponse>
   */
  deleteGame(id: string, options?: msRest.RequestOptionsBase): Promise<msRest.RestResponse>;
  /**
   * @param id
   * @param callback The callback
   */
  deleteGame(id: string, callback: msRest.ServiceCallback<void>): void;
  /**
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  deleteGame(id: string, options: msRest.RequestOptionsBase, callback: msRest.ServiceCallback<void>): void;
  deleteGame(id: string, options?: msRest.RequestOptionsBase | msRest.ServiceCallback<void>, callback?: msRest.ServiceCallback<void>): Promise<msRest.RestResponse> {
    return this.sendOperationRequest(
      {
        id,
        options
      },
      deleteGameOperationSpec,
      callback);
  }

  /**
   * @summary Queries a User
   * @param [options] The optional parameters
   * @returns Promise<msRest.RestResponse>
   */
  queryUsers(options?: Models.WwaRestApiClientQueryUsersOptionalParams): Promise<msRest.RestResponse>;
  /**
   * @param callback The callback
   */
  queryUsers(callback: msRest.ServiceCallback<void>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  queryUsers(options: Models.WwaRestApiClientQueryUsersOptionalParams, callback: msRest.ServiceCallback<void>): void;
  queryUsers(options?: Models.WwaRestApiClientQueryUsersOptionalParams | msRest.ServiceCallback<void>, callback?: msRest.ServiceCallback<void>): Promise<msRest.RestResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      queryUsersOperationSpec,
      callback);
  }

  /**
   * @summary Gets a list of Users
   * @param [options] The optional parameters
   * @returns Promise<Models.GetUsersResponse>
   */
  getUsers(options?: Models.WwaRestApiClientGetUsersOptionalParams): Promise<Models.GetUsersResponse>;
  /**
   * @param callback The callback
   */
  getUsers(callback: msRest.ServiceCallback<Models.UserSummaryViewModel[]>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  getUsers(options: Models.WwaRestApiClientGetUsersOptionalParams, callback: msRest.ServiceCallback<Models.UserSummaryViewModel[]>): void;
  getUsers(options?: Models.WwaRestApiClientGetUsersOptionalParams | msRest.ServiceCallback<Models.UserSummaryViewModel[]>, callback?: msRest.ServiceCallback<Models.UserSummaryViewModel[]>): Promise<Models.GetUsersResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      getUsersOperationSpec,
      callback) as Promise<Models.GetUsersResponse>;
  }

  /**
   * @summary Creates a User
   * @param [options] The optional parameters
   * @returns Promise<Models.CreateUserResponse>
   */
  createUser(options?: Models.WwaRestApiClientCreateUserOptionalParams): Promise<Models.CreateUserResponse>;
  /**
   * @param callback The callback
   */
  createUser(callback: msRest.ServiceCallback<Models.UserReadViewModel>): void;
  /**
   * @param options The optional parameters
   * @param callback The callback
   */
  createUser(options: Models.WwaRestApiClientCreateUserOptionalParams, callback: msRest.ServiceCallback<Models.UserReadViewModel>): void;
  createUser(options?: Models.WwaRestApiClientCreateUserOptionalParams | msRest.ServiceCallback<Models.UserReadViewModel>, callback?: msRest.ServiceCallback<Models.UserReadViewModel>): Promise<Models.CreateUserResponse> {
    return this.sendOperationRequest(
      {
        options
      },
      createUserOperationSpec,
      callback) as Promise<Models.CreateUserResponse>;
  }

  /**
   * @summary Gets a User
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<Models.GetUserResponse>
   */
  getUser(id: string, options?: msRest.RequestOptionsBase): Promise<Models.GetUserResponse>;
  /**
   * @param id
   * @param callback The callback
   */
  getUser(id: string, callback: msRest.ServiceCallback<Models.UserReadViewModel>): void;
  /**
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  getUser(id: string, options: msRest.RequestOptionsBase, callback: msRest.ServiceCallback<Models.UserReadViewModel>): void;
  getUser(id: string, options?: msRest.RequestOptionsBase | msRest.ServiceCallback<Models.UserReadViewModel>, callback?: msRest.ServiceCallback<Models.UserReadViewModel>): Promise<Models.GetUserResponse> {
    return this.sendOperationRequest(
      {
        id,
        options
      },
      getUserOperationSpec,
      callback) as Promise<Models.GetUserResponse>;
  }

  /**
   * @summary Replaces a User
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<Models.ReplaceUserResponse>
   */
  replaceUser(id: string, options?: Models.WwaRestApiClientReplaceUserOptionalParams): Promise<Models.ReplaceUserResponse>;
  /**
   * @param id
   * @param callback The callback
   */
  replaceUser(id: string, callback: msRest.ServiceCallback<Models.UserReadViewModel>): void;
  /**
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  replaceUser(id: string, options: Models.WwaRestApiClientReplaceUserOptionalParams, callback: msRest.ServiceCallback<Models.UserReadViewModel>): void;
  replaceUser(id: string, options?: Models.WwaRestApiClientReplaceUserOptionalParams | msRest.ServiceCallback<Models.UserReadViewModel>, callback?: msRest.ServiceCallback<Models.UserReadViewModel>): Promise<Models.ReplaceUserResponse> {
    return this.sendOperationRequest(
      {
        id,
        options
      },
      replaceUserOperationSpec,
      callback) as Promise<Models.ReplaceUserResponse>;
  }

  /**
   * @summary Deletes a User
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<msRest.RestResponse>
   */
  deleteUser(id: string, options?: msRest.RequestOptionsBase): Promise<msRest.RestResponse>;
  /**
   * @param id
   * @param callback The callback
   */
  deleteUser(id: string, callback: msRest.ServiceCallback<void>): void;
  /**
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  deleteUser(id: string, options: msRest.RequestOptionsBase, callback: msRest.ServiceCallback<void>): void;
  deleteUser(id: string, options?: msRest.RequestOptionsBase | msRest.ServiceCallback<void>, callback?: msRest.ServiceCallback<void>): Promise<msRest.RestResponse> {
    return this.sendOperationRequest(
      {
        id,
        options
      },
      deleteUserOperationSpec,
      callback);
  }

  /**
   * @summary Queries a World Map
   * @param gameId
   * @param [options] The optional parameters
   * @returns Promise<msRest.RestResponse>
   */
  queryWorldMaps(gameId: string, options?: Models.WwaRestApiClientQueryWorldMapsOptionalParams): Promise<msRest.RestResponse>;
  /**
   * @param gameId
   * @param callback The callback
   */
  queryWorldMaps(gameId: string, callback: msRest.ServiceCallback<void>): void;
  /**
   * @param gameId
   * @param options The optional parameters
   * @param callback The callback
   */
  queryWorldMaps(gameId: string, options: Models.WwaRestApiClientQueryWorldMapsOptionalParams, callback: msRest.ServiceCallback<void>): void;
  queryWorldMaps(gameId: string, options?: Models.WwaRestApiClientQueryWorldMapsOptionalParams | msRest.ServiceCallback<void>, callback?: msRest.ServiceCallback<void>): Promise<msRest.RestResponse> {
    return this.sendOperationRequest(
      {
        gameId,
        options
      },
      queryWorldMapsOperationSpec,
      callback);
  }

  /**
   * @summary Gets a list of World Maps
   * @param gameId
   * @param [options] The optional parameters
   * @returns Promise<Models.GetWorldMapsResponse>
   */
  getWorldMaps(gameId: string, options?: Models.WwaRestApiClientGetWorldMapsOptionalParams): Promise<Models.GetWorldMapsResponse>;
  /**
   * @param gameId
   * @param callback The callback
   */
  getWorldMaps(gameId: string, callback: msRest.ServiceCallback<Models.WorldMapSummaryViewModel[]>): void;
  /**
   * @param gameId
   * @param options The optional parameters
   * @param callback The callback
   */
  getWorldMaps(gameId: string, options: Models.WwaRestApiClientGetWorldMapsOptionalParams, callback: msRest.ServiceCallback<Models.WorldMapSummaryViewModel[]>): void;
  getWorldMaps(gameId: string, options?: Models.WwaRestApiClientGetWorldMapsOptionalParams | msRest.ServiceCallback<Models.WorldMapSummaryViewModel[]>, callback?: msRest.ServiceCallback<Models.WorldMapSummaryViewModel[]>): Promise<Models.GetWorldMapsResponse> {
    return this.sendOperationRequest(
      {
        gameId,
        options
      },
      getWorldMapsOperationSpec,
      callback) as Promise<Models.GetWorldMapsResponse>;
  }

  /**
   * @summary Creates a World Map
   * @param gameId
   * @param [options] The optional parameters
   * @returns Promise<Models.CreateWorldMapResponse>
   */
  createWorldMap(gameId: string, options?: Models.WwaRestApiClientCreateWorldMapOptionalParams): Promise<Models.CreateWorldMapResponse>;
  /**
   * @param gameId
   * @param callback The callback
   */
  createWorldMap(gameId: string, callback: msRest.ServiceCallback<Models.WorldMapReadViewModel>): void;
  /**
   * @param gameId
   * @param options The optional parameters
   * @param callback The callback
   */
  createWorldMap(gameId: string, options: Models.WwaRestApiClientCreateWorldMapOptionalParams, callback: msRest.ServiceCallback<Models.WorldMapReadViewModel>): void;
  createWorldMap(gameId: string, options?: Models.WwaRestApiClientCreateWorldMapOptionalParams | msRest.ServiceCallback<Models.WorldMapReadViewModel>, callback?: msRest.ServiceCallback<Models.WorldMapReadViewModel>): Promise<Models.CreateWorldMapResponse> {
    return this.sendOperationRequest(
      {
        gameId,
        options
      },
      createWorldMapOperationSpec,
      callback) as Promise<Models.CreateWorldMapResponse>;
  }

  /**
   * @summary Gets a World Map
   * @param gameId
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<Models.GetWorldMapResponse>
   */
  getWorldMap(gameId: string, id: string, options?: msRest.RequestOptionsBase): Promise<Models.GetWorldMapResponse>;
  /**
   * @param gameId
   * @param id
   * @param callback The callback
   */
  getWorldMap(gameId: string, id: string, callback: msRest.ServiceCallback<Models.WorldMapReadViewModel>): void;
  /**
   * @param gameId
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  getWorldMap(gameId: string, id: string, options: msRest.RequestOptionsBase, callback: msRest.ServiceCallback<Models.WorldMapReadViewModel>): void;
  getWorldMap(gameId: string, id: string, options?: msRest.RequestOptionsBase | msRest.ServiceCallback<Models.WorldMapReadViewModel>, callback?: msRest.ServiceCallback<Models.WorldMapReadViewModel>): Promise<Models.GetWorldMapResponse> {
    return this.sendOperationRequest(
      {
        gameId,
        id,
        options
      },
      getWorldMapOperationSpec,
      callback) as Promise<Models.GetWorldMapResponse>;
  }

  /**
   * @summary Updates a World Map
   * @param gameId
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<Models.UpdateWorldMapResponse>
   */
  updateWorldMap(gameId: string, id: string, options?: Models.WwaRestApiClientUpdateWorldMapOptionalParams): Promise<Models.UpdateWorldMapResponse>;
  /**
   * @param gameId
   * @param id
   * @param callback The callback
   */
  updateWorldMap(gameId: string, id: string, callback: msRest.ServiceCallback<Models.WorldMapReadViewModel>): void;
  /**
   * @param gameId
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  updateWorldMap(gameId: string, id: string, options: Models.WwaRestApiClientUpdateWorldMapOptionalParams, callback: msRest.ServiceCallback<Models.WorldMapReadViewModel>): void;
  updateWorldMap(gameId: string, id: string, options?: Models.WwaRestApiClientUpdateWorldMapOptionalParams | msRest.ServiceCallback<Models.WorldMapReadViewModel>, callback?: msRest.ServiceCallback<Models.WorldMapReadViewModel>): Promise<Models.UpdateWorldMapResponse> {
    return this.sendOperationRequest(
      {
        gameId,
        id,
        options
      },
      updateWorldMapOperationSpec,
      callback) as Promise<Models.UpdateWorldMapResponse>;
  }

  /**
   * @summary Deletes a World Map
   * @param gameId
   * @param id
   * @param [options] The optional parameters
   * @returns Promise<msRest.RestResponse>
   */
  deleteWorldMap(gameId: string, id: string, options?: msRest.RequestOptionsBase): Promise<msRest.RestResponse>;
  /**
   * @param gameId
   * @param id
   * @param callback The callback
   */
  deleteWorldMap(gameId: string, id: string, callback: msRest.ServiceCallback<void>): void;
  /**
   * @param gameId
   * @param id
   * @param options The optional parameters
   * @param callback The callback
   */
  deleteWorldMap(gameId: string, id: string, options: msRest.RequestOptionsBase, callback: msRest.ServiceCallback<void>): void;
  deleteWorldMap(gameId: string, id: string, options?: msRest.RequestOptionsBase | msRest.ServiceCallback<void>, callback?: msRest.ServiceCallback<void>): Promise<msRest.RestResponse> {
    return this.sendOperationRequest(
      {
        gameId,
        id,
        options
      },
      deleteWorldMapOperationSpec,
      callback);
  }
}

// Operation Specifications
const serializer = new msRest.Serializer(Mappers);
const testAccessTokenOperationSpec: msRest.OperationSpec = {
  httpMethod: "HEAD",
  path: "accessTokens",
  responses: {
    204: {},
    401: {},
    403: {},
    500: {},
    default: {}
  },
  serializer
};

const createAccessTokenOperationSpec: msRest.OperationSpec = {
  httpMethod: "POST",
  path: "accessTokens",
  requestBody: {
    parameterPath: [
      "options",
      "body"
    ],
    mapper: Mappers.AccessTokenCreateViewModel
  },
  responses: {
    200: {
      bodyMapper: {
        serializedName: "parsedResponse",
        type: {
          name: "String"
        }
      }
    },
    400: {},
    401: {},
    403: {},
    500: {},
    default: {}
  },
  serializer
};

const getCurrentContextOperationSpec: msRest.OperationSpec = {
  httpMethod: "GET",
  path: "currentContext",
  responses: {
    200: {
      bodyMapper: Mappers.CurrentContextReadViewModel
    },
    401: {},
    403: {},
    500: {},
    default: {}
  },
  serializer
};

const queryGamesOperationSpec: msRest.OperationSpec = {
  httpMethod: "HEAD",
  path: "games",
  queryParameters: [
    Parameters.name
  ],
  responses: {
    204: {},
    400: {},
    401: {},
    403: {},
    500: {},
    default: {}
  },
  serializer
};

const getGamesOperationSpec: msRest.OperationSpec = {
  httpMethod: "GET",
  path: "games",
  queryParameters: [
    Parameters.skip,
    Parameters.take,
    Parameters.sortField,
    Parameters.sortDirection,
    Parameters.search
  ],
  responses: {
    200: {
      bodyMapper: {
        serializedName: "parsedResponse",
        type: {
          name: "Sequence",
          element: {
            type: {
              name: "Composite",
              className: "GameSummaryViewModel"
            }
          }
        }
      },
      headersMapper: Mappers.GetGamesHeaders
    },
    400: {
      headersMapper: Mappers.GetGamesHeaders
    },
    401: {
      headersMapper: Mappers.GetGamesHeaders
    },
    403: {
      headersMapper: Mappers.GetGamesHeaders
    },
    500: {
      headersMapper: Mappers.GetGamesHeaders
    },
    default: {}
  },
  serializer
};

const createGameOperationSpec: msRest.OperationSpec = {
  httpMethod: "POST",
  path: "games",
  requestBody: {
    parameterPath: [
      "options",
      "body"
    ],
    mapper: Mappers.GameCreateViewModel
  },
  responses: {
    200: {
      bodyMapper: Mappers.GameReadViewModel
    },
    400: {},
    401: {},
    403: {},
    500: {},
    default: {}
  },
  serializer
};

const getGameOperationSpec: msRest.OperationSpec = {
  httpMethod: "GET",
  path: "games/{id}",
  urlParameters: [
    Parameters.id
  ],
  responses: {
    200: {
      bodyMapper: Mappers.GameReadViewModel
    },
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const updateGameOperationSpec: msRest.OperationSpec = {
  httpMethod: "PATCH",
  path: "games/{id}",
  urlParameters: [
    Parameters.id
  ],
  requestBody: {
    parameterPath: [
      "options",
      "body"
    ],
    mapper: {
      serializedName: "body",
      type: {
        name: "Sequence",
        element: {
          type: {
            name: "Composite",
            className: "Operation"
          }
        }
      }
    }
  },
  responses: {
    200: {
      bodyMapper: Mappers.GameReadViewModel
    },
    400: {},
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const deleteGameOperationSpec: msRest.OperationSpec = {
  httpMethod: "DELETE",
  path: "games/{id}",
  urlParameters: [
    Parameters.id
  ],
  responses: {
    204: {},
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const queryUsersOperationSpec: msRest.OperationSpec = {
  httpMethod: "HEAD",
  path: "users",
  queryParameters: [
    Parameters.email
  ],
  responses: {
    204: {},
    400: {},
    401: {},
    403: {},
    500: {},
    default: {}
  },
  serializer
};

const getUsersOperationSpec: msRest.OperationSpec = {
  httpMethod: "GET",
  path: "users",
  queryParameters: [
    Parameters.skip,
    Parameters.take,
    Parameters.sortField,
    Parameters.sortDirection,
    Parameters.search
  ],
  responses: {
    200: {
      bodyMapper: {
        serializedName: "parsedResponse",
        type: {
          name: "Sequence",
          element: {
            type: {
              name: "Composite",
              className: "UserSummaryViewModel"
            }
          }
        }
      },
      headersMapper: Mappers.GetUsersHeaders
    },
    400: {
      headersMapper: Mappers.GetUsersHeaders
    },
    401: {
      headersMapper: Mappers.GetUsersHeaders
    },
    403: {
      headersMapper: Mappers.GetUsersHeaders
    },
    500: {
      headersMapper: Mappers.GetUsersHeaders
    },
    default: {}
  },
  serializer
};

const createUserOperationSpec: msRest.OperationSpec = {
  httpMethod: "POST",
  path: "users",
  requestBody: {
    parameterPath: [
      "options",
      "body"
    ],
    mapper: Mappers.UserCreateViewModel
  },
  responses: {
    200: {
      bodyMapper: Mappers.UserReadViewModel
    },
    400: {},
    401: {},
    403: {},
    500: {},
    default: {}
  },
  serializer
};

const getUserOperationSpec: msRest.OperationSpec = {
  httpMethod: "GET",
  path: "users/{id}",
  urlParameters: [
    Parameters.id
  ],
  responses: {
    200: {
      bodyMapper: Mappers.UserReadViewModel
    },
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const replaceUserOperationSpec: msRest.OperationSpec = {
  httpMethod: "PUT",
  path: "users/{id}",
  urlParameters: [
    Parameters.id
  ],
  requestBody: {
    parameterPath: [
      "options",
      "body"
    ],
    mapper: Mappers.UserReplaceViewModel
  },
  responses: {
    200: {
      bodyMapper: Mappers.UserReadViewModel
    },
    400: {},
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const deleteUserOperationSpec: msRest.OperationSpec = {
  httpMethod: "DELETE",
  path: "users/{id}",
  urlParameters: [
    Parameters.id
  ],
  responses: {
    204: {},
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const queryWorldMapsOperationSpec: msRest.OperationSpec = {
  httpMethod: "HEAD",
  path: "games/{gameId}/worldMaps",
  urlParameters: [
    Parameters.gameId
  ],
  queryParameters: [
    Parameters.name
  ],
  responses: {
    204: {},
    400: {},
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const getWorldMapsOperationSpec: msRest.OperationSpec = {
  httpMethod: "GET",
  path: "games/{gameId}/worldMaps",
  urlParameters: [
    Parameters.gameId
  ],
  queryParameters: [
    Parameters.skip,
    Parameters.take,
    Parameters.sortField,
    Parameters.sortDirection,
    Parameters.search
  ],
  responses: {
    200: {
      bodyMapper: {
        serializedName: "parsedResponse",
        type: {
          name: "Sequence",
          element: {
            type: {
              name: "Composite",
              className: "WorldMapSummaryViewModel"
            }
          }
        }
      },
      headersMapper: Mappers.GetWorldMapsHeaders
    },
    400: {
      headersMapper: Mappers.GetWorldMapsHeaders
    },
    401: {
      headersMapper: Mappers.GetWorldMapsHeaders
    },
    403: {
      headersMapper: Mappers.GetWorldMapsHeaders
    },
    404: {
      headersMapper: Mappers.GetWorldMapsHeaders
    },
    500: {
      headersMapper: Mappers.GetWorldMapsHeaders
    },
    default: {}
  },
  serializer
};

const createWorldMapOperationSpec: msRest.OperationSpec = {
  httpMethod: "POST",
  path: "games/{gameId}/worldMaps",
  urlParameters: [
    Parameters.gameId
  ],
  requestBody: {
    parameterPath: [
      "options",
      "body"
    ],
    mapper: Mappers.WorldMapCreateViewModel
  },
  responses: {
    200: {
      bodyMapper: Mappers.WorldMapReadViewModel
    },
    400: {},
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const getWorldMapOperationSpec: msRest.OperationSpec = {
  httpMethod: "GET",
  path: "games/{gameId}/worldMaps/{id}",
  urlParameters: [
    Parameters.gameId,
    Parameters.id
  ],
  responses: {
    200: {
      bodyMapper: Mappers.WorldMapReadViewModel
    },
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const updateWorldMapOperationSpec: msRest.OperationSpec = {
  httpMethod: "PATCH",
  path: "games/{gameId}/worldMaps/{id}",
  urlParameters: [
    Parameters.gameId,
    Parameters.id
  ],
  requestBody: {
    parameterPath: [
      "options",
      "body"
    ],
    mapper: {
      serializedName: "body",
      type: {
        name: "Sequence",
        element: {
          type: {
            name: "Composite",
            className: "Operation"
          }
        }
      }
    }
  },
  responses: {
    200: {
      bodyMapper: Mappers.WorldMapReadViewModel
    },
    400: {},
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

const deleteWorldMapOperationSpec: msRest.OperationSpec = {
  httpMethod: "DELETE",
  path: "games/{gameId}/worldMaps/{id}",
  urlParameters: [
    Parameters.gameId,
    Parameters.id
  ],
  responses: {
    204: {},
    401: {},
    403: {},
    404: {},
    500: {},
    default: {}
  },
  serializer
};

export {
  WwaRestApiClient,
  WwaRestApiClientContext,
  Models as WwaRestApiModels,
  Mappers as WwaRestApiMappers
};
