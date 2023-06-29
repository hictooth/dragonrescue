using System;

namespace dragonrescue.Schema;

public enum SubscriptionNotificationType
{
	NONE,
	SUBSCRIPTION_IN_GRACE_PERIOD,
	SUBSCRIPTION_EXPIRED,
	SUBSCRIPTION_ON_HOLD,
	SUBSCRIPTION_RECOVERED,
	SUBSCRIPTION_CANCELED
}
