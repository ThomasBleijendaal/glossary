using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Implements;

namespace MediatorCommandHandler.Requests;

internal record WhistleMagic(Whistle RequestModel) : Request<Whistle, Damage>(RequestModel);
