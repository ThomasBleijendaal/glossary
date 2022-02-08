using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Implements;

namespace MediatorCommandHandler.Requests;

internal record RodMagic(Rod RequestModel) : Request<Rod, Damage>(RequestModel);
