using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Implements;

namespace MediatorCommandHandler.Requests;

internal record FanMagic(Fan RequestModel) : Request<Fan, Damage>(RequestModel);
