using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Implements;

namespace MediatorCommandHandler.Requests;

internal record StaffMagic(Staff RequestModel) : Request<Staff, Damage>(RequestModel);
