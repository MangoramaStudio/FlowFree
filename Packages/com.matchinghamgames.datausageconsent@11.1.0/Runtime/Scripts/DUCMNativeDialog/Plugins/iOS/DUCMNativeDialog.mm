
#ifdef UNITY_4_0 || UNITY_5_0
#import "iPhone_View.h"
#else
extern UIViewController* UnityGetGLViewController();
#endif


// Credit: https://github.com/ChrisMaire/unity-native-sharing


@interface UNativeInputDialog:NSObject
+ (void)alertDialog:(NSString *)paragraph1 ppText:(NSString *)pptext tcText:(NSString *)tctext ppInfo:(NSString *)pplink tcInfo:(NSString *)tclink pressOkText:(NSString *)pressoktext okText:(NSString *)oktext gameObject:(NSString *)gameobject methodName:(NSString *)method;
@end

@implementation UNativeInputDialog

+ (void)alertDialog:(NSString *)paragraph1 ppText:(NSString *)pptext tcText:(NSString *)tctext ppInfo:(NSString *)pplink tcInfo:(NSString *)tclink pressOkText:(NSString *)pressoktext okText:(NSString *)oktext gameObject:(NSString *)gameobject methodName:(NSString *)method {
	UIAlertController * alert=   [UIAlertController
                                  alertControllerWithTitle:nil
                                  message:@"\n\n\n\n\n\n\n\n"
                                  preferredStyle:UIAlertControllerStyleAlert];
    
    
        
        UIAlertAction* ok = [UIAlertAction actionWithTitle:oktext style:UIAlertActionStyleDefault
                                                   handler:^(UIAlertAction * action) {

                                                       UnitySendMessage([self getCString:gameobject], [self getCString:method], "");
                                                   }];
        
        [alert addAction:ok];
        
    UIViewController *rootViewController = UnityGetGLViewController();
    [rootViewController presentViewController:alert animated:YES completion:^(void) {
        UITextView *textView = [[UITextView alloc] initWithFrame:CGRectMake(16, 32, alert.view.frame.size.width - 32, 160)];
            
        textView.backgroundColor = [UIColor clearColor];
        textView.editable = NO;
        textView.selectable = YES;
        textView.scrollEnabled = YES;
        textView.textAlignment = NSTextAlignmentCenter;
        
        textView.dataDetectorTypes = UIDataDetectorTypeLink;   
        NSURL *tc_url = [NSURL URLWithString: tclink];
        NSURL *pp_url = [NSURL URLWithString: pplink];
        
        NSMutableDictionary *normalAttribute = [NSMutableDictionary dictionary];
        NSMutableDictionary *boldAttribute = [NSMutableDictionary dictionary];

        NSMutableParagraphStyle *paraStyle = [[NSMutableParagraphStyle alloc] init];
        paraStyle.alignment = NSTextAlignmentCenter;

        [normalAttribute setObject:paraStyle forKey:NSParagraphStyleAttributeName];
        [normalAttribute setObject:[UIFont systemFontOfSize:14] forKey:NSFontAttributeName];

        [boldAttribute setObject:paraStyle forKey:NSParagraphStyleAttributeName];
        [boldAttribute setObject:[UIFont boldSystemFontOfSize:16] forKey:NSFontAttributeName];

        NSMutableDictionary *tc_linkAttribute = [NSMutableDictionary dictionary];
        [tc_linkAttribute setObject:[UIFont boldSystemFontOfSize:16] forKey:NSFontAttributeName];
        [tc_linkAttribute setObject:[UIColor blackColor] forKey:NSForegroundColorAttributeName];
        [tc_linkAttribute setObject:[NSNumber numberWithInt:NSUnderlineStyleSingle] forKey:NSUnderlineStyleAttributeName];
        [tc_linkAttribute setObject: tc_url forKey:NSLinkAttributeName];

        NSMutableDictionary *pp_linkAttribute = [NSMutableDictionary dictionary];
        [pp_linkAttribute setObject:[UIFont boldSystemFontOfSize:16] forKey:NSFontAttributeName];
        [pp_linkAttribute setObject:[UIColor blackColor] forKey:NSForegroundColorAttributeName];
        [pp_linkAttribute setObject:[NSNumber numberWithInt:NSUnderlineStyleSingle] forKey:NSUnderlineStyleAttributeName];
        [pp_linkAttribute setObject: pp_url forKey:NSLinkAttributeName];

        NSMutableAttributedString *baseText = [[NSMutableAttributedString alloc]initWithString:paragraph1 attributes:boldAttribute];
		// Replace {1} with tctext
		NSRange range1 = [baseText.mutableString rangeOfString:@"{1}"];
		if (range1.location != NSNotFound) {
		[baseText replaceCharactersInRange:range1 withString:tctext];
		
		// Add tc_linkAttribute to the replaced tctext
		NSRange tcTextRange = NSMakeRange(range1.location, [tctext length]);
		[baseText setAttributes:tc_linkAttribute range:tcTextRange];
		}
		
		// Replace {2} with pptext
		NSRange range2 = [baseText.mutableString rangeOfString:@"{2}"];
		if (range2.location != NSNotFound) {
		[baseText replaceCharactersInRange:range2 withString:pptext];
		
		// Add pp_linkAttribute to the replaced pptext
		NSRange ppTextRange = NSMakeRange(range2.location, [pptext length]);
		[baseText setAttributes:pp_linkAttribute range:ppTextRange];
		}
        NSMutableAttributedString *lastLine = [[NSMutableAttributedString alloc] initWithString:pressoktext attributes:normalAttribute];
        [baseText appendAttributedString:lastLine];
        textView.attributedText = baseText;
        
        UITraitCollection *currentTraitCollection = alert.traitCollection;
        if (currentTraitCollection.userInterfaceStyle == UIUserInterfaceStyleDark) {
            textView.textColor = [UIColor whiteColor];
            textView.linkTextAttributes = @{NSForegroundColorAttributeName:[UIColor whiteColor]};
        }
        else
        {
            textView.textColor = [UIColor blackColor];
            textView.linkTextAttributes = @{NSForegroundColorAttributeName:[UIColor blackColor]};
        }
        
        [alert.view addSubview:textView];
}];
}

// Credit: https://stackoverflow.com/a/37052118/2373034
+ (char *)getCString:(NSString *)source {
    if (source == nil)
        source = @"";
    
    const char *sourceUTF8 = [source UTF8String];
    char *result = (char*) malloc(strlen(sourceUTF8) + 1);
    strcpy(result, sourceUTF8);
    
    return result;
}

@end

extern "C" void _DUCMNativeDialog_AlertDialog(const char* paragraph1, const char* pptext, const char* tctext, const char* pplink, const char* tclink, const char* pressoktext, const char* oktext, const char* gameobject, const char* method ) {
    [UNativeInputDialog alertDialog:[NSString stringWithUTF8String:paragraph1] ppText:[NSString stringWithUTF8String:pptext] tcText:[NSString stringWithUTF8String:tctext] ppInfo:[NSString stringWithUTF8String:pplink] tcInfo:[NSString stringWithUTF8String:tclink] pressOkText:[NSString stringWithUTF8String:pressoktext] okText:[NSString stringWithUTF8String:oktext] gameObject:[NSString stringWithUTF8String:gameobject] methodName:[NSString stringWithUTF8String:method]];
}
