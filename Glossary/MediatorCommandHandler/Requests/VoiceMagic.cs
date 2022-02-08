using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Implements;

namespace MediatorCommandHandler.Requests;

internal record VoiceMagic(Voice RequestModel) : Request<Voice, Damage>(RequestModel);
