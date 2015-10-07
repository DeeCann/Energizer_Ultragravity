using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;

namespace Soomla.Store {
	public class BuyPacksAsset : IStoreAssets {

		public int GetVersion() {
			return 0;
		}
		
		// NOTE: Even if you have no use in one of these functions, you still need to
		// implement them all and just return an empty array.
		
		public VirtualCurrency[] GetCurrencies() {
			return new VirtualCurrency[]{LEVEL_PACKS_CURRENCY};
		}
		
		public VirtualGood[] GetGoods() {
			return new VirtualGood[] {PREMIUM_PACKS};
		}
		
		public VirtualCurrencyPack[] GetCurrencyPacks() {
			return new VirtualCurrencyPack[] {};
		}
		
		public VirtualCategory[] GetCategories() {
			return new VirtualCategory[]{GENERAL_CATEGORY};
		}
		
		/** Virtual Currencies **/
		
		public static VirtualCurrency LEVEL_PACKS_CURRENCY = new VirtualCurrency(
			"PACK",                            // Name
			"Level pack",                      // Description
			"level_pack_ID"                    // Item ID
			);
		
		/** Virtual Currency Packs **/
		public static VirtualCurrencyPack THREE_LEVEL_PACK = new VirtualCurrencyPack(
			"3 Level Packs",                          		// Name
			"3 Level packs with 15 levels each",            // Description
			"level_packs_3",                       			// Item ID
			3,                                  			// Number of currencies in the pack
			"level_pack_ID",                   				// ID of the currency associated with this pack
			new PurchaseWithMarket(               			// Purchase type (with real money $)
				"level_packs_3",             				// Product ID
		        7.99                         				// Price (in real money $)
		        )
			);

		
		// NOTE: Create non-consumable items using LifeTimeVG with PurchaseType of PurchaseWithMarket.
		public static VirtualGood PREMIUM_PACKS = new LifetimeVG(
			"3 Premium Packs",                             						// Name
			"Brand new 3 level packs with 15 levels each",                      // Description
			"Premium_Packs",                          							// Item ID
			new PurchaseWithMarket(               								// Purchase type (with real money $)
				"Premium_Packs",                      							// Product ID
		        7.99                                   							// Price (in real money $)
		        )
			);
		
		/** Virtual Categories **/
		
		public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
			"General", new List<string>(new string[] {PREMIUM_PACKS.ToString()})
			);
		
	}
}